using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Zenject.SpaceFighter;

public class NewTestScript
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene(0);
    }

    [UnityTest]
    public IEnumerator PlayerCreated()
    {
        yield return new WaitForSeconds(2);
        CharacterData _character = UnityEngine.Object.FindObjectOfType<CharacterData>();

        UnityEngine.Assertions.Assert.IsNotNull(_character);
    }

    [UnityTest]
    public IEnumerator EnemiesCreated()
    {
        yield return new WaitForSeconds(2);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        UnityEngine.Assertions.Assert.AreEqual(34, enemies.Length);
    }

    [UnityTest]
    public IEnumerator PlayerCanShoot()
    {
        yield return new WaitForSeconds(2);
        CharacterData _character = UnityEngine.Object.FindObjectOfType<CharacterData>();
        ShootAbility sha = _character.gameObject.GetComponent<ShootAbility>();
        sha.Execute();
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        UnityEngine.Assertions.Assert.AreEqual(1, bullets.Length);
    }

    [UnityTest]
    public IEnumerator HummerIsAnimating()
    {
        yield return new WaitForSeconds(1);
        HummerAnimation _hummer = UnityEngine.Object.FindObjectOfType<HummerAnimation>();
        Quaternion startRotation = _hummer.transform.localRotation;
        yield return new WaitForSeconds(1);
        Quaternion endRotation = _hummer.transform.localRotation;
        UnityEngine.Assertions.Assert.AreNotEqual(startRotation, endRotation);
    }

    [UnityTest]
    public IEnumerator PlayerSettings()
    {
        yield return new WaitForSeconds(1);
        PlayerSettings[] settings = Resources.LoadAll<PlayerSettings>("PlayerSettings");

        UnityEngine.Assertions.Assert.AreEqual(2, settings.Length);
    }
}
