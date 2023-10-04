using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
//using NSubstitute;
using RedditFun;
using NSubstitute.Extensions;
using NuGet.Frameworks;

namespace RedditFun.Tests
{
	[TestClass]
	public class RedditFun
	{
		[TestMethod]
		public void RedditFun_LogInfo_Test_Success()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Info(It.IsAny<string>()));

			mock.Object.Info(It.IsAny<string>());
		}

		[TestMethod]
		public void RedditFun_LogInfo_Test_FailedFileNotFound()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Info(It.IsAny<string>())).Throws(new FileNotFoundException("File not found.", "Application.log"));

			try
			{
				mock.Object.Info(It.IsAny<string>());
			}
			catch (FileNotFoundException ex)
			{
				Assert.AreEqual(ex.Message, "File not found.");
				Assert.AreEqual(ex.FileName, "Application.log");
			}
		}

		[TestMethod]
		public void RedditFun_LogInfo_Test_FailedPermissions()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Info(It.IsAny<string>())).Throws(new IOException("Access denied."));

			try
			{
				mock.Object.Info(It.IsAny<string>());
			}
			catch (IOException ex)
			{
				Assert.AreEqual(ex.Message, "Access denied.");
			}
		}


		[TestMethod]
		public void RedditFun_LogDebug_Test_Success()
		{
			var mock = new Mock<ILogger>();

			//It.IsAny<string>
			mock.Setup(t => t.Debug(It.IsAny<string>()));

			mock.Object.Debug(It.IsAny<string>());

		}

		[TestMethod]
		public void RedditFun_LogDebug_Test_FailedFileNotFound()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Debug(It.IsAny<string>())).Throws(new FileNotFoundException("File not found.", "Application.log"));

			try
			{
				mock.Object.Debug(It.IsAny<string>());
			}
			catch (FileNotFoundException ex)
			{
				Assert.AreEqual(ex.Message, "File not found.");
				Assert.AreEqual(ex.FileName, "Application.log");
			}
		}

		[TestMethod]
		public void RedditFun_LogDebug_Test_FailedPermissions()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Debug(It.IsAny<string>())).Throws(new IOException("Access denied."));

			try
			{
				mock.Object.Debug(It.IsAny<string>());
			}
			catch (IOException ex)
			{
				Assert.AreEqual(ex.Message, "Access denied.");
			}
		}

		[TestMethod]
		public void RedditFun_LogError_Test_Success()
		{
			var mock = new Mock<ILogger>();

			//It.IsAny<string>
			mock.Setup(t => t.Error(It.IsAny<string>(), It.IsAny<Exception>()));

			mock.Object.Error(It.IsAny<string>(), It.IsAny<Exception>());

		}

		[TestMethod]
		public void RedditFun_LogError_Test_FailedFileNotFound()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Error(It.IsAny<string>(), It.IsAny<Exception>())).Throws(new FileNotFoundException("File not found.", "Application.log"));

			try
			{
				mock.Object.Debug(It.IsAny<string>());
			}
			catch (FileNotFoundException ex)
			{
				Assert.AreEqual(ex.Message, "File not found.");
				Assert.AreEqual(ex.FileName, "Application.log");
			}
		}

		[TestMethod]
		public void RedditFun_LogError_Test_FailedPermissions()
		{
			var mock = new Mock<ILogger>();

			mock.Setup(t => t.Error(It.IsAny<string>(), It.IsAny<Exception>())).Throws(new IOException("Access denied."));

			try
			{
				mock.Object.Error(It.IsAny<string>());
			}
			catch (IOException ex)
			{
				Assert.AreEqual(ex.Message, "Access denied.");
			}
		}
	}
}