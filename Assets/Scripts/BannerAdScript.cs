using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdScript : MonoBehaviour
{

    public string gameId = "4195891";
    public string surfacingId = "Banner_Android";
    public bool testMode = false;

    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowBannerWhenInitialized());
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(surfacingId);
    }
}