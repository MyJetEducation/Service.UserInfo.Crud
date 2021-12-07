namespace Service.UserInfo.Crud.Services
{
	public interface IEncoderDecoder
	{
		string Encode(string str);
		string Decode(string str);
		string Hash(string str);
	}
}