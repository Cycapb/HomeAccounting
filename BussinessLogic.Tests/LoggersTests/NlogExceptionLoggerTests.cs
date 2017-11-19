using System;
using System.Collections.Generic;
using BussinessLogic.Loggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loggers.Models;

namespace BussinessLogic.Tests.LoggersTests
{
    [TestClass]
    public class NlogExceptionLoggerTests
    {
        private readonly NlogExceptionLogger _nlogExceptionLogger;

        public NlogExceptionLoggerTests()
        {
            _nlogExceptionLogger = new NlogExceptionLogger();
        }

        [TestMethod]
        [TestCategory("NLogExceptionLoggerTests")]
        public void LogException_InputNullMvcLoggingModel_Returns()
        {
            _nlogExceptionLogger.LogException(null, null);
        }

        [TestMethod]
        [TestCategory("NLogExceptionLoggerTests")]
        public void LogException_InputNullException_Returns()
        {
            _nlogExceptionLogger.LogException(null, new MvcLoggingModel());
        }

        [TestMethod]
        [TestCategory("NLogExceptionLoggerTests")]
        public void Logexception_InputNotNullExceptionAndMvcLoggingModelRouteData()
        {
            var exception = new Exception("Test Exception");
            var mvcLoggingModel = new MvcLoggingModel()
            {
                RouteData = new Dictionary<string, object>()
                {
                    {"controller", "Controller"},
                    {"action", "Action"}
                }
            };

            _nlogExceptionLogger.LogException(exception, mvcLoggingModel);
        }

        [TestMethod]
        [TestCategory("NLogExceptionLoggerTests")]
        public void Logexception_InputMvcLoggingModelRouteDataIsNull()
        {
            var exception = new Exception("Test Exception");
            var mvcLoggingModel = new MvcLoggingModel();

            _nlogExceptionLogger.LogException(exception, mvcLoggingModel);
        }

        [TestMethod]
        [TestCategory("NLogExceptionLoggerTests")]
        public void Logexception_InputMvcLoggingModelRouteDataWithWrongKeys()
        {
            var exception = new Exception("Test Exception");
            var mvcLoggingModel = new MvcLoggingModel()
            {
                RouteData = new Dictionary<string, object>()
                {
                    {"fake", "Fake"}
                }
            };

            _nlogExceptionLogger.LogException(exception, mvcLoggingModel);
        }

        [TestMethod]
        [TestCategory("NLogExceptionLoggerTests")]
        public void Logexception_InputExceptionWithInnerException()
        {
            var exception = new Exception("Main_Exception", new Exception("Inner_Exception"));
            var mvcLoggingModel = new MvcLoggingModel();

            _nlogExceptionLogger.LogException(exception, mvcLoggingModel);
        }
    }
}
