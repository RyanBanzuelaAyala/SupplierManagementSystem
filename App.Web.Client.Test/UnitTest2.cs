using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App.Web.Client.Test
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tempFile = @"D:\330071_0009585229_KHB_00153_NFD";

            new eDNB.POBL.Utilities.PDF().ConvertToPDFPO(tempFile + ".txt", tempFile + ".pdf", "Danube.jpg");

        }
    }
}
