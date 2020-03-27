﻿using GlyssenEngine;
using GlyssenFileBasedPersistence;
using NUnit.Framework;

/// <summary>
/// Assembly-level one-time setup (https://github.com/nunit/nunit/issues/1577)
/// </summary>
[SetUpFixture]
public class TestFixtureSetUp
{
	[OneTimeSetUp]
	public void OneTimeSetUp()
	{
		var persistenceImpl = new PersistenceImplementation();
		ProjectBase.Reader = persistenceImpl;
		ReferenceTextProxy.Reader = persistenceImpl;
		Project.Writer = persistenceImpl;
	}
}
