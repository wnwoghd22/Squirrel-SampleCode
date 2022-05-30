using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class ShareManager : MonoBehaviour
{
	public static string TEST_LINK = "https://play.google.com/store/apps/details?id=com.DefaultCompany.squirrel";

	IMailHandler mailHandler;

	public const string INSTAGRAM = "com.instagram.android";
	public const string FACEBOOK = "com.facebook.katana";
	public const string TWITTER = "com.twitter.android";

	[SerializeField] private Sprite[] shareImage;

    private void Awake()
    {
		mailHandler = FindObjectOfType<MailManager>();
    }

    void Update()
	{
		//if (Input.GetMouseButtonDown(0))
		//	StartCoroutine(TakeScreenshotAndShare());
	}

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		//string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		//File.WriteAllBytes(filePath, ss.EncodeToPNG());

		string filePath = FileManager.WriteTextureFile(ss, "shared img.png");

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject("Subject goes here").SetText("Hello world!").SetUrl("https://github.com/yasirkula/UnityNativeShare")
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();

		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}

    public void Test() => UniTask.Create(async () => await ShareScreenShotTestAsync());
    private async UniTask ShareScreenShotTestAsync()
	{
		// 화면이 완전히 그려지기를 기다립니다.
		await UniTask.WaitForEndOfFrame(this);

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = FileManager.WriteTextureFile(ss, "shared img.png");

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject("test").SetText("detail").SetUrl(TEST_LINK)
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();

	}

	public void ShareScreenShot(Achievement achievement) => UniTask.Create(async () => await ShareScreenShotAsync(achievement));
	private async UniTask ShareScreenShotAsync(Achievement achievement)
    {
		// 화면이 완전히 그려지기를 기다립니다.
		await UniTask.WaitForEndOfFrame(this);

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = FileManager.WriteTextureFile(ss, "shared img.png");

		// To avoid memory leaks
		Destroy(ss);

		/*new NativeShare().AddFile(filePath)
			.SetSubject(achievement.name).SetText(achievement.detail).SetUrl(TEST_LINK)
			.SetCallback((result, shareTarget) => SendMail(result, shareTarget))
			.Share();*/
		new NativeShare().AddFile(filePath)
			.SetSubject(achievement.name).SetUrl(TEST_LINK)
			.SetCallback((result, shareTarget) => SendMail(result, shareTarget))
			.Share();
	}

	private void SendMail(NativeShare.ShareResult result, string shareTarget)
    {
		Debug.Log("Share result: " + result + ", selected app: " + shareTarget);

		if (result == NativeShare.ShareResult.Shared)
		{
			// mailHandler.Send(new Mail("광고비", eReward.INITSTAGE, 1));

			mailHandler.Send(new Mail("SNS 공유 보상", eReward.ACORN, 250));
		}
	}

	public async UniTask ShareScreenShotThroughPlatformAsync(Achievement achievement, string platform)
    {
		// 화면이 완전히 그려지기를 기다립니다.
		await UniTask.WaitForEndOfFrame(this);

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = FileManager.WriteTextureFile(ss, "shared img.png");

		// To avoid memory leaks
		Destroy(ss);

		if (NativeShare.TargetExists(platform))
			new NativeShare().AddFile(filePath)
			.SetSubject(achievement.name + " 엔딩을 획득했다람!")
			.SetText("다음 모험도 함께 해달람!")
			.SetUrl(TEST_LINK).AddTarget(platform)
			.SetCallback((result, shareTarget) =>
			{
				if (!achievement.shared)
                {
					achievement.shared = true;
					SendMail(result, shareTarget);
                }
			})
			.Share();
	}

	public void ShareImage(Achievement achievement, string platform)
	{
		Debug.Log("제발아라아아ㅏㅇ" + achievement.index);
		// reference: https://support.unity.com/hc/en-us/articles/206486626-How-can-I-get-pixels-from-unreadable-textures-

		Texture2D texture = shareImage[achievement.index].texture;

		// Create a temporary RenderTexture of the same size as the texture
		RenderTexture tmp = RenderTexture.GetTemporary(
							texture.width,
							texture.height,
							0,
							RenderTextureFormat.Default,
							RenderTextureReadWrite.Linear);

		// Blit the pixels on texture to the RenderTexture
		Graphics.Blit(texture, tmp);

		// Backup the currently set RenderTexture
		RenderTexture previous = RenderTexture.active;

		// Set the current RenderTexture to the temporary one we created
		RenderTexture.active = tmp;

		// Create a new readable Texture2D to copy the pixels to it
		Texture2D myTexture2D = new Texture2D(texture.width, texture.height);

		// Copy the pixels from the RenderTexture to the new Texture
		myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
		myTexture2D.Apply();

		// Reset the active RenderTexture
		RenderTexture.active = previous;

		// Release the temporary RenderTexture
		RenderTexture.ReleaseTemporary(tmp);

		string filePath = FileManager.WriteTextureFile(myTexture2D, "shared img.png");

		if (NativeShare.TargetExists(platform))
			new NativeShare().AddFile(filePath)
			.SetSubject(achievement.name + " 엔딩을 획득했다람!")
			.SetText("다음 모험도 함께 해달람!")
			.SetUrl(TEST_LINK).AddTarget(platform)
			.SetCallback((result, shareTarget) =>
			{
				if (!achievement.shared)
				{
					achievement.shared = true;
					SendMail(result, shareTarget);
				}
			})
			.Share();
	}
}
