using UnityEngine;
using System.Collections;

namespace Steinmetz
{
  // loads remote texture into object
  public class RemoteImage : MonoBehaviour
  {
    // what we do once image is loaded
    public delegate void OnLoaded();
    public OnLoaded OnLoadedProxy;

    protected int width;
    protected int height;
    protected int imageId = 0;

    private string link = null;
    private bool isLoading = false;

    public void Start()
    {
      this.OnLoadedProxy = Resize;
    }

    public void SetImageId(int id)
    {
      this.imageId = id;
    }

    public void SetLink(string link)
    {
      this.link = link;
    }

    protected void Resize()
    {
      transform.localScale = new Vector3(this.width * 0.001f, this.height * 0.001f, 1);
    }

    public void LoadImage()
    {
      if (this.isLoading)
        return;

      StartCoroutine(this.LoadImageCoroutine());
    }

    public bool IsLoading()
    {
      return this.isLoading;
    }

    private IEnumerator LoadImageCoroutine()
    {
      if (this.link == null)
        yield return false;

      this.isLoading = true;

      WWW www = new WWW(this.link);

      yield return www;

      Renderer renderer = GetComponent<Renderer>();

      // it's useful to know w/h
      this.width  = www.texture.width;
      this.height = www.texture.height;

      renderer.material.mainTexture = www.texture;

      DestroyImmediate(www.texture);
      www.Dispose();
      www = null;

      Resources.UnloadUnusedAssets();

      this.isLoading = false;

      if (this.OnLoadedProxy != null)
        this.OnLoadedProxy();
    }
  }
}
