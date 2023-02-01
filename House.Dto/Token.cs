namespace House.Dto
{
    public class Token
    {
        public Token()
        {
            Code = 200;
            Message = "操作成功";
        }

        public int Code { get; set; }

        public string Message { get; set; }
    }

    public class Token<T> : Token
    {
        /// <summary>
        /// 回传的结果
        /// </summary>
        public T Result { get; set; }
    }
}