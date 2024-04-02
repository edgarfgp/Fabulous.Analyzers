namespace Fabulous.Analyzer.Tests


open FSharp.Analyzers.SDK.Testing
open Fabulous.Analyzers
open Xunit

module NestedLayoutsAnalyzerTests =
    [<Fact>]
    let ``Nested layouts analyzer should report an error when there are nested layouts`` () =
        async {            
            let! projectOptions =
                mkOptionsFromProject
                    "net8.0"
                    [
                        {
                        Name = "Fabulous"
                        Version = "3.0.0-pre2"
                        }
                        {
                            Name = "Fabulous.Avalonia"
                            Version = "3.0.0-pre2" 
                        }

                    ]
                |> Async.AwaitTask
 
            let source =
                """
                
namespace VStackTests

open Fabulous
open Fabulous.Avalonia
open type Fabulous.Avalonia.View                

module Tests =
    let view() =
        VStack() {
            VStack() {
                Label("Hello")
                Label("World")
            }
        }
        """
            let ctx = getContext projectOptions source
            let! res = NestedLayoutsAnalyzer.nestedLayoutsAnalyzer ctx
            Assert.NotNull res
        }