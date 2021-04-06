using NUnit.Framework;
using Velentr.Logging.Helpers;

namespace Velentr.Logging.Test
{
    public class FormatStringTest
    {

        public class MockClass
        {

            public int Latitude;

            public string Longitude;

            public override string ToString()
            {
                return $"Lat: {Latitude}, Lon: {Longitude}";
            }

        }

        [Test]
        public void TestBasicString()
        {
            var result = StringHelpers.FormatString("Hello World!");

            Assert.AreEqual(result, "Hello World!");
        }

        [Test]
        public void TestBasicStringFormatting()
        {
            var world = "World!";
            var result = StringHelpers.FormatString("Hello {World}. I am a {turtle}, but I am also a {world}. There are {turtle}s all the way down.", world, "turtle");

            Assert.AreEqual(result, "Hello World!. I am a turtle, but I am also a World!. There are turtles all the way down.");
        }

        [Test]
        public void TestStringSerialization()
        {
            var coordinates = new MockClass()
            {
                Latitude = 25,
                Longitude = "124.421"
            };
            var elapsedTime = 34;

            var result = StringHelpers.FormatString("Processed {@Coordinates} in {Elapsed} ms. Coordinates: {Coordinates}", coordinates, elapsedTime);

            Assert.AreEqual(result, "Processed {\"Latitude\":25,\"Longitude\":\"124.421\"}} in 34 ms. Coordinates: Lat: 25, Lon: 124.421");
        }
    }
}
