using BotService;

namespace UnitTests
{
    [TestClass]
    public sealed class BotAnswer
    {
        [TestMethod]
        public void TestMethod1()
        {
            string expected = "Привет";
            string input = "Hello";

            string answer = BOT.SendMessage(input).Message;

            Assert.AreEqual(expected, answer);
        }
        [TestMethod]
        public void TestMethod2()
        {
            string expected = "Как дела";
            string input = "Normalno";

            string answer = BOT.SendMessage(input).Message;

            Assert.AreEqual(expected, answer);
        }
        [TestMethod]
        public void TestMethod3()
        {
            string expected = "Команды не существует";
            string input = ":3";

            string answer = BOT.SendMessage(input).Message;

            Assert.AreEqual(expected, answer);
        }
    }
}
