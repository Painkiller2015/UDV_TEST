

namespace BotService
{
    public static class BOT
    {
        private static readonly Dictionary<string, string> answerDict = new();
        static BOT()
        {
            answerDict.Add("Hello", "Привет");
            answerDict.Add("Normalno", "Как дела");
        }
        public static Answer SendMessage(string Message)
        {
            Answer answer = new
            (
                Message = answerDict.GetValueOrDefault(Message) ?? "Команды не существует"
            );

            return answer;
        }
        public class Answer
        {
            public Answer(string message)
            {
                Message = message;
                Date = DateTime.Now;
            }

            public string Message { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
