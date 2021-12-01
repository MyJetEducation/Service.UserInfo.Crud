using MyJetWallet.Sdk.Postgres;

namespace Service.UserInfo.Crud.Postgres.DesignTime
{
	public class ContextFactory : MyDesignTimeContextFactory<DatabaseContext>
	{
		public ContextFactory() : base(options => new DatabaseContext(options))
		{
		}
	}
}