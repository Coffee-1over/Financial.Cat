namespace Financial.Cat.Domain.Models.Business.AuthModels.Auth
{
    public class AuthStartOperationResult
    {
		public AuthStartOperationResult()
        {

        }
		public AuthStartOperationResult(long operationId, string code)
        {
            OperationId = operationId;
            Code = code;
        }
        public long OperationId { get; set; }
        public string Code { get; set; }
    }
}
