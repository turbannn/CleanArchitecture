using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;
using NetArchTest.Rules;

namespace ArchitectureTests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string PresentationNamespace = "Presentation";
    private const string WebNamespace = "Web";

    [Test]
    public void Domain_Should_Not_Depend_On_Other_Projects()
    {
        //Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        var otherProjects = new[] { ApplicationNamespace, InfrastructureNamespace, PresentationNamespace, WebNamespace };

        //Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        Assert.That(result.IsSuccessful, Is.True);
    }

    [Test]
    public void Application_Should_Not_Depend_On_Upper_Projects()
    {
        //Arrange
        var assembly = Application.AssemblyReference.Assembly;

        var otherProjects = new[] { InfrastructureNamespace, PresentationNamespace, WebNamespace };

        //Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        //Assert
        Assert.That(result.IsSuccessful, Is.True);
    }

    [Test]
    public void Presentation_Should_Not_Depend_On_Web()
    {
        //Arrange
        var assembly = Presentation.AssemblyReference.Assembly;

        //Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(WebNamespace)
            .GetResult();

        //Assert
        Assert.That(result.IsSuccessful, Is.True);
    }

    [Test]
    public void Infrastructure_Should_Not_Depend_On_Web()
    {
        //Arrange
        var assembly = Infrastructure.AssemblyReference.Assembly;

        //Act
        var result = Types.InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn(WebNamespace)
            .GetResult();

        //Assert
        Assert.That(result.IsSuccessful, Is.True);
    }

    [Test]
    public void Handlers_Should_Depend_On_Domain()
    {
        //Arrange
        var assembly = Application.AssemblyReference.Assembly;

        //Act
        var result = Types.InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        //Assert
        Assert.That(result.IsSuccessful, Is.True);
    }
}
