namespace PipServices.Commons.Validate
{
    public class ValidationResult
    {
        public ValidationResult() { }

        public ValidationResult(string path, ValidationResultType type,
            string code, string message, object expected, object actual)
        {
            Path = path;
            Type = type;
            Code = code;
            Message = message;
        }

        public string Path { get; set; }
        public ValidationResultType Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object Expected { get; set; }
        public object Actual { get; set; }
    }
}
