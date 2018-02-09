using System;
using FluentAssertions;
using SinjulMSBH_Blog.Models;
using Xunit;

namespace SinjulMSBH_Blog.Test.Models
{
	public class HelloTests
	{
		[Fact]
		public void HelloManShouldBeWellFormated ( )
		{
			// Arrange
			var hello = new Hello("Sinjul", "MSBH");

			//Act
			var helloMan = hello.HelloMan();

			//Assert
			helloMan
			    .Should( )
			    .StartWith( "Hello" )
			    .And
			    .EndWith( "!" )
			    .And
			    .Contain( "Sinjul" )
			    .And
			    .Contain( "MSBH" );
		}

		[Fact]
		public void HelloManShouldBeRaiseExceptionWhenFirstNameIsNotSet ( )
		{
			// Arrange
			var hello = new Hello("", "MSBH");

			//Act
			Action actionHelloMan = () => hello.HelloMan();

			//Assert
			actionHelloMan
			    .Should( )
			    .Throw<MissingFirstNameException>( )
			    .WithMessage( "FirstName is missing" );
		}
	}
}