using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.UserInfo.Crud.Settings
{
    public class SettingsModel
    {
        [YamlProperty("UserInfo.Crud.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("UserInfo.Crud.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("UserInfo.Crud.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("UserInfo.Crud.PostgresConnectionString")]
        public string PostgresConnectionString { get; set; }
    }
}
