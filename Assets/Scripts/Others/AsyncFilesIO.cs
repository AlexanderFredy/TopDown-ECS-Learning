using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Jobs;
using System.Text;
using Unity.Collections;
using UniRx;
using Zenject;

namespace AsyncGetFile
{

    [Serializable]
    public class PlayerSettings
    {
        [field: SerializeField] private int startHealth;
        [field: SerializeField] private float movespeed;
        [field: SerializeField] private float invisDuration;
        [field: SerializeField] private float invisCooldown;

        public void Synchronize()
        {
            var resSpeed = GameObject.FindObjectsOfType<UserInputData>();
            for (int i = 0; i < resSpeed.Length; i++)
            {
                resSpeed[i].speed = movespeed;
            }

            var resHealth = GameObject.FindObjectsOfType<CharacterHealth>();
            for (int i = 0; i < resHealth.Length; i++)
            {
                if (resHealth[i].GetComponent<UserInputData>())
                    resHealth[i].SetHealth(startHealth);
            }

            var resInvis = GameObject.FindObjectsOfType<InvisibleAbility>();
            for (int i = 0; i < resInvis.Length; i++)
            {
                resInvis[i].duration = invisDuration;
                resInvis[i].collDownDuration = invisCooldown;
            }
        }

        public float GetMoveSpeed() => movespeed;
    }

    public class AsyncFilesIO : MonoBehaviour
    {
        string _fileName = "Settings";

        public void CreateFile()
        {
            FileStream fs = new FileStream("Assets/Resources/" + _fileName + ".json", FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.Write(JsonUtility.ToJson(new PlayerSettings()));
            writer.Close();
            writer.Dispose();
        }

        public async Task GetSettings(TextAsset jsonText)
        {
            PlayerSettings settings = JsonUtility.FromJson<PlayerSettings>(jsonText.text);
            settings.Synchronize();

            await Task.Yield();
        }

        public void GetSettingsSync()
        {
            float startTime = Time.time;

            TextAsset jsonText = Resources.Load<TextAsset>(_fileName);

            // GetSettings(jsonText);

            Debug.Log("Sync duration " + (Time.time - startTime));
        }

        public async void GetSettingsAsyncAwait()
        {
            float startTime = Time.time;

            TextAsset jsonText = Resources.Load<TextAsset>(_fileName);

            await GetSettings(jsonText);

            Debug.Log("Async await duration " + (Time.time - startTime));
        }

        public void GetSettingsUniRx()
        {
            float startTime = Time.time;

            Resources.LoadAsync<TextAsset>(_fileName)
                .AsAsyncOperationObservable()
                .Subscribe(xs =>
                {
                    if (xs.asset != null)
                    {
                        //GetSettings(xs.asset as TextAsset);
                        Debug.Log("UniRx duration " + (Time.time - startTime));
                    }
                }).AddTo(this);
        }

        public void GetSettingsJobs()
        {
            TextAsset jsonText = Resources.Load<TextAsset>(_fileName);

            GetSettingsJob job = new GetSettingsJob
            {
                jsonTextArray = new NativeArray<byte>(jsonText.bytes, Allocator.Persistent),
                movespeed = new NativeArray<float>(1, Allocator.Persistent)
            };

            JobHandle handle = job.Schedule();
            handle.Complete();

            Debug.Log("Move speed: " + job.movespeed[0]);
            job.jsonTextArray.Dispose();
            job.movespeed.Dispose();
        }
    }

    public struct GetSettingsJob : IJob
    {
        public NativeArray<byte> jsonTextArray;
        public NativeArray<float> movespeed;

        public void Execute()
        {
            string jsonText = Encoding.ASCII.GetString(jsonTextArray.ToArray());

            PlayerSettings settings = JsonUtility.FromJson<PlayerSettings>(jsonText);

            movespeed[0] = settings.GetMoveSpeed();
        }
    }

}


