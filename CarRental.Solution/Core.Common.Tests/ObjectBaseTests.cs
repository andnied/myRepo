using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Tests.TestClasses;
using System.ComponentModel;

namespace Core.Common.Tests
{
    [TestClass]
    public class ObjectBaseTests
    {
        [TestMethod]
        public void Property_changed_triggers_event()
        {
            var testClass = new DerivedObjectBaseClass();
            var propertyChanged = false;

            testClass.PropertyChanged += (o, e) =>
            {
                propertyChanged = true;
            };

            testClass.TestProperty = "Test";

            Assert.IsTrue(propertyChanged);
        }

        [TestMethod]
        public void Class_IsDirty_returns_false()
        {
            var testClass = new DerivedObjectBaseClass();

            Assert.IsFalse(testClass.IsDirty);

            testClass.TestProperty = "blabla";

            Assert.IsTrue(testClass.IsDirty);
        }

        [TestMethod]
        public void Class_doesnt_allow_duplicate_event_subscribers()
        {
            var counter = 0;
            var testClass = new DerivedObjectBaseClass();
            var handler = new PropertyChangedEventHandler((o, e) => { counter++; });

            testClass.PropertyChanged += handler;
            testClass.PropertyChanged += handler;

            testClass.TestProperty = "test";

            Assert.AreEqual(counter, 1);
        }

        [TestMethod]
        public void Class_IsValid_returns_true()
        {
            var testClass = new DerivedObjectBaseClass();

            Assert.IsFalse(testClass.IsValid);

            testClass.TestProperty = "test";

            Assert.IsTrue(testClass.IsValid);
        }
    }
}
