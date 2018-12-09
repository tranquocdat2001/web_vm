namespace PublicModel
{
    public class MessageModel
    {
        public MessageModel()
        {
            Message = "";
            Error = false;
        }
        public int Id { get; set; }
        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Có lỗi hay không có lỗi
        /// </summary>
        public bool Error { get; set; }
        public int NextAction { get; set; }
        public int DelayTime { get; set; } //đơn vị milisecon 

    }
}
