using CoreBusinessLogic;
using CoreDomain;
using System;
using Xunit;

namespace XUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void GetCardByImage_Proper2CImage_ShouldReturnCardObject()
        {
            var cardReco = new CardRecognition();
            var cardManager = new CardManager(cardReco);

            var res = cardManager.GetCardByImage("C:\\Users\\mkosi\\Documents\\GitHub\\Poker\\RunPy\\XUnitTests\\Resources\\2c.png");

            Assert.NotNull(res);
            Assert.Equal("2C", res.Name);
        }
    }
}
