public class FileConfigUnit : FileConfig<FileConfigUnitRecord>
{
    public override void DefineConfigCompare()
    {
        configCompare =  new ConfigCompare<FileConfigUnitRecord>();
    }
}
