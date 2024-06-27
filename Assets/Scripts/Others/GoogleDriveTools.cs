using UnityEngine;
using System.Collections.Generic;
using System.Text;
//using UnityGoogleDrive;
//using UnityGoogleDrive.Data;
using System.Threading.Tasks;

public static class GoogleDriveTools
{
    //public static List<File> FileList()
    //{
    //    List<File> output = new List<File>();
    //    GoogleDriveFiles.List().Send().OnDone += fileList => { output = fileList.Files; };
    //    return output;
    //}

    //public async static Task<File> Upload(string obj, string id)//, Action onDone)
    //{
    //    var file = new File { Content = Encoding.ASCII.GetBytes(obj) };

    //    if (id == "")
    //    {
    //        //сначала получаем релевантный ID файла от Гугла
    //        var request = GoogleDriveFiles.GenerateIds(1);
    //        await request.Send();

    //        file.Id = request.ResponseData.Ids[0];

    //        file.Name = "GameData_" + file.Id + ".json";
    //        PlayerPrefs.SetString("IdSetting", file.Id);

    //        await GoogleDriveFiles.Create(file).Send();
    //    } else
    //    {
    //        await GoogleDriveFiles.Update(id, file).Send();
    //    }
     
    //    return file;
    //}

    //public async static Task<File> Download(string fileId)
    //{
    //    File output = new File();
    //    output = await GoogleDriveFiles.Download(fileId).Send();//.OnDone += file => { output = file; };

    //    return output;
    //}
}
