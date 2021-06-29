namespace My.Core.Interfaces
{
    public interface IDecoderEncoderService
    {
        string Decode(string input);
        string Encode(string input);
        string HTMLDecode(string input);
        string HTMLEncode(string input);
    }
}