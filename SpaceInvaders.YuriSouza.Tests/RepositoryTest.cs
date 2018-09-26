using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpaceInvaders.YuriSouza.Entities;
using SpaceInvaders.YuriSouza.Repository;
using SpaceInvaders.YuriSouza.Utility;

namespace SpaceInvaders.YuriSouza.Tests
{
    [TestClass]
    public class RepositoryTest
    {

        [TestMethod]
        public void InsertScore()
        {
            var repository = new RepositoryMemory();

            repository.Insert(10);

            var expected = repository.Get().ToArray()[0];

            Assert.AreEqual(10, expected);
        }

        [TestMethod]
        public void GetGoodScores()
        {
            var repository = new RepositoryMemory();

            for (int i = 0; i < 10; i++)
            {
                var expected = new Random().Next(1, 100) + 1;
                repository.Insert(expected);

                var result = repository.Get().ToArray()[i];

                Assert.AreEqual(result, expected);
            }
        }
    }


}
