namespace BE_ThuyDuong.PayLoad.Response
{
    public class ResponseObject<T>
    {
        public int Status {  get; set; }
        public string Message { get; set; }
        public T Data {  get; set; }
        


        public ResponseObject() { }

        public ResponseObject(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public ResponseObject<T> ResponseObjectSucces (string message, T data)
        {
            return new ResponseObject<T>(StatusCodes.Status200OK, message, data);
        }
        public ResponseObject<T> ResponseObjectError (int error,string message, T data)
        {
            return new ResponseObject<T>(error, message, data);
        }

    }
}
