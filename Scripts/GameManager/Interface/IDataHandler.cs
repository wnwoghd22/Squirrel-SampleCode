using Cysharp.Threading.Tasks;

public interface IDataHandler : IPropertyHandler
{
    public DataManager DataManager { get; }
    public SaveData Data { get; }

    UniTask<ObjData[]> GetObjsAsync(int chapter, int stage);
}
