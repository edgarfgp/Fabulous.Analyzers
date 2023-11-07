module Fabulous.Analyzer.Tests.NestedLayoutsAnalyzerTests

open FSharp.Compiler.CodeAnalysis
open Fabulous.Analyzers
open NUnit.Framework
open FSharp.Analyzers.SDK.Testing

let mutable projectOptions: FSharpProjectOptions = FSharpProjectOptions.zero

[<SetUp>]
let Setup () =
    task {
        let! opts =
            mkOptionsFromProject
                "net8.0"
                [
                    {
                        Name = "Fabulous"
                        Version = "2.4.0"
                    }
                    {
                        Name = "Fabulous.Avalonia"
                        Version = "2.0.0-pre16" 
                    }
                    {
                        Name = "Fantomas.FCS"
                        Version = "6.2.0"
                    }
                ]

        projectOptions <- opts
    }

[<Test>]
let ``Nested layouts analyzer should report an error when there are nested layouts`` () =
    async {
        let source =
            """
module View
open Fabulous.Avalonia
open type Fabulous.Avalonia.View

let view() =
    VStack() {
        VStack() {
            TextBlock("Hello")
            TextBlock("World")
        }
    }
    """
        let ctx = getContext projectOptions source
        let! msgs = NestedLayoutsAnalyzer.nestedLayoutsAnalyzer ctx
        Assert.IsNotEmpty msgs
        Assert.IsTrue(Assert.messageContains "" msgs[0])
    }