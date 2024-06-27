using System.Collections;
using UnityEngine;
//using UnityGoogleDrive.Data;
using System.Threading.Tasks;
using Unity.Entities;
using Zenject;

    public class GameManager : MonoBehaviour, IInitializable
    {
        private DiContainer _diContainer;
        private GameSettings _gameSettings;
        private InventoryController _inventoryController;

        public PlayerSettings PlayerSettings { get; set; }

        private Entity _playerWeaponEntity;

        private GameObject _currentEnemyKnight;
        private GameObject _currentEnemyWarlock;

        private GameObject _health1;
        private float TimeInstanceHealth1;
        private GameObject _health2;
        private float TimeInstanceHealth2;

        public GameObject CurrentPlayer { get; private set; }
        public bool PlayerIsDead { get; private set; }

        [Inject]
        public void Construct(DiContainer diContainer, GameSettings gameSettings, InventoryController inventoryController)
        {
            _diContainer = diContainer;
            _gameSettings = gameSettings;
            _inventoryController = inventoryController;
        }

        public void Initialize()
        {
            //LoadUserDataFromServer();
            InitializePlayerSetting();

            StartCoroutine(SpawnHeroEnemiesPowerUps());
        }

        private void InitializePlayerSetting()
        {
            UnityEngine.Object[] settings = Resources.LoadAll("PlayerSettings");

            this.PlayerSettings = (PlayerSettings)settings[0];
        }

        private async void CreateNewPlayer()
        {
            CurrentPlayer = _diContainer.InstantiatePrefab(_gameSettings.playerPrefab, new Vector3(0, 0, -4.3f), Quaternion.identity, null);
            CurrentPlayer.GetComponent<CharacterHealth>().OnKill += KillPlayer;
            CurrentPlayer.GetComponent<CharacterHealth>().OnGetHit += () => CurrentPlayer.GetComponent<Animator>().SetTrigger("GetHit");
            PlayerIsDead = false;
            SetPlayerStats();
            UserIndicators.S.SubscribeOnPlayer(CurrentPlayer);

            WeaponAbility swordPrefab = CurrentPlayer.GetComponent<MeleeAbility>().curWeapon;
            WeaponAbility playerSword = await GetPlayerWeapon(swordPrefab);
            _playerWeaponEntity = playerSword.MyEntity;
            playerSword.transform.parent = CurrentPlayer.transform.Find("root/pelvis/Weapon");
            playerSword.transform.localPosition = swordPrefab.HandlerLocalPos + new Vector3(-0.08f, -0.03f, -0.03f); ;
            playerSword.transform.localEulerAngles = swordPrefab.HandlerLocalRot + new Vector3(0, 0, -82f);
        }

        public void SetPlayerStats()
        {
            if (CurrentPlayer.TryGetComponent(out CharacterHealth hp))
                hp.SetHealth(PlayerSettings.startHealth);
            if (CurrentPlayer.TryGetComponent(out UserInputData uid))
                uid.speed = PlayerSettings.movespeed;
            if (CurrentPlayer.TryGetComponent(out InvisibleAbility inv))
            {
                inv.duration = PlayerSettings.invisDuration;
                inv.collDownDuration = PlayerSettings.invisCooldown;
            }
        }

        private async Task<WeaponAbility> GetPlayerWeapon(WeaponAbility prefab)
        {
            var playerSword = Instantiate(prefab, new Vector3(100, -100, 100), Quaternion.identity);
            playerSword.transform.localScale *= 1.2f;
            playerSword.IsBullet = false;

            if (playerSword.gameObject.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            playerSword.SetCollisionLayerAndOwner(CurrentPlayer);

            await Task.Delay(1000);

            return playerSword;
        }

        void OnApplicationQuit()
        {
            //SaveUserDataToServer();
        }

        private void KillPlayer()
        {
            PlayerIsDead = true;
            CurrentPlayer.GetComponent<Animator>().SetTrigger("Die");
            CurrentPlayer.GetComponent<CharacterHealth>().OnKill -= KillPlayer;
            CurrentPlayer.GetComponent<CharacterHealth>().OnGetHit -= () => CurrentPlayer.GetComponent<Animator>().SetTrigger("GetHit");

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            entityManager.DestroyEntity(_playerWeaponEntity);

            UserInputData uid = CurrentPlayer.GetComponent<UserInputData>();
            InputData data = entityManager.GetComponentData<InputData>(uid.MyEntity);
            data.IsDead = true;
            entityManager.SetComponentData(uid.MyEntity, data);

            UserIndicators.S.UnSubscribeOnPlayer();
            _inventoryController.ClearInventory();

            Destroy(CurrentPlayer, 4f);
        }

        //private async void LoadUserDataFromServer()
        //{
        //    string fileID = PlayerPrefs.GetString("IdSetting");

        //    if (!fileID.Equals(string.Empty, StringComparison.Ordinal))
        //    {
        //        File f = await GoogleDriveTools.Download(fileID);
        //        stat = stat.FromExport(Encoding.ASCII.GetString(f.Content));

        //        //поскольку сам показатель здоровья хранится отдельно, мы его здесь и обновляем
        //        if (_player != null)
        //            _player.GetComponent<CharacterHealth>().Health = stat.health <= 0 ? 100 : stat.health;
        //    }
        //    else
        //        stat = new();
        //}

        //private async void SaveUserDataToServer()
        //{
        //    string fileID = PlayerPrefs.GetString("IdSetting");
        //    File f = await GoogleDriveTools.Upload(stat.ToExport(), fileID);   
        //}

        private IEnumerator SpawnHeroEnemiesPowerUps()
        {
            if (_gameSettings.enemySpawner != null)
                _diContainer.InstantiatePrefab(_gameSettings.enemySpawner, _gameSettings.enemySpawner.position, Quaternion.identity, null);


            while (true)
            {
                if (_gameSettings.enemyPrefabKnight != null && _gameSettings.enemySpawner != null && _currentEnemyKnight == null)
                {
                    _currentEnemyKnight = _diContainer.InstantiatePrefab(_gameSettings.enemyPrefabKnight, _gameSettings.enemySpawner.position, Quaternion.identity, null);
                }

                if (_gameSettings.enemyPrefabWarlock != null && _currentEnemyWarlock == null)
                {
                    _currentEnemyWarlock = _diContainer.InstantiatePrefab(_gameSettings.enemyPrefabWarlock, Utils.GetPositionInArea(), Quaternion.identity, null);
                }

                if (_gameSettings.playerPrefab != null && CurrentPlayer == null)
                {
                    CreateNewPlayer();
                }

                if (_gameSettings.healthPrefab != null)
                {
                    if (_health1 == null)
                    {
                        if (Time.time - TimeInstanceHealth1 > 10f)
                        {
                            _health1 = _diContainer.InstantiatePrefab(_gameSettings.healthPrefab, new Vector3(-5.4f, 0.5f, -3.6f), Quaternion.identity, null);
                            TimeInstanceHealth1 = Time.time;
                        }
                    }

                    if (_health2 == null)
                    {
                        if (Time.time - TimeInstanceHealth2 > 10f)
                        {
                            _health2 = _diContainer.InstantiatePrefab(_gameSettings.healthPrefab, new Vector3(4.65f, 0.5f, -3.6f), Quaternion.identity, null);
                            TimeInstanceHealth2 = Time.time;
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
