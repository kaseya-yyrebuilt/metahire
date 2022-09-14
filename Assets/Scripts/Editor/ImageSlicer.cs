using System.IO;
using UnityEditor;
using UnityEngine;

public static class ImageSlicer
{
    [MenuItem("Assets/ImageSlicer/Process to Sprites")]
    private static void ProcessToSprite()
    {
        var image = Selection.activeObject as Texture2D; //获取旋转的对象
        var rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(image)); //获取路径名称
        var path = rootPath + "/" + image.name + ".PNG"; //图片路径名称


        var texImp = AssetImporter.GetAtPath(path) as TextureImporter; //获取图片入口


        AssetDatabase.CreateFolder(rootPath, image.name); //创建文件夹


        foreach (var metaData in texImp.spritesheet) //遍历小图集
        {
            var myimage = new Texture2D((int) metaData.rect.width, (int) metaData.rect.height);

            //abc_0:(x:2.00, y:400.00, width:103.00, height:112.00)
            for (var y = (int) metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++) //Y轴像素
            {
                for (var x = (int) metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                    myimage.SetPixel(x - (int) metaData.rect.x, y - (int) metaData.rect.y, image.GetPixel(x, y));
            }


            //转换纹理到EncodeToPNG兼容格式
            if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
            {
                var newTexture = new Texture2D(myimage.width, myimage.height);
                newTexture.SetPixels(myimage.GetPixels(0), 0);
                myimage = newTexture;
            }

            var pngData = myimage.EncodeToPNG();


            //AssetDatabase.Create
            //Asset(myimage, rootPath + "/" + image.name + "/" + metaData.name + ".PNG");
            File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".PNG", pngData);
            // 刷新资源窗口界面
            AssetDatabase.Refresh();
        }
    }
}