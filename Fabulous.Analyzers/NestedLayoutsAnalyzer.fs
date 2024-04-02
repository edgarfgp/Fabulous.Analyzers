namespace Fabulous.Analyzers

open System
open FSharp.Analyzers.SDK
open FSharp.Analyzers.SDK.TASTCollecting
open FSharp.Compiler.Text

module NestedLayoutsAnalyzer =

    [<CliAnalyzer("NestedLayoutsAnalyzer",
              "Analyzes the code for nested layouts",
              "")>]
    let nestedLayoutsAnalyzer: Analyzer<CliContext> =
        fun (ctx: CliContext) ->
            async {
                let state = ResizeArray<range>()

                let walker =
                        { new TypedTreeCollectorBase() with
                            override _.WalkLet _  _ _ =
                                ()
       
                        }
                
                match ctx.TypedTree with
                | None -> ()
                | Some typedTree -> walkTast walker typedTree

                return
                    state
                    |> Seq.map (fun r ->
                        {
                            Type = "NestedLayoutsAnalyzer"
                            Message = "Nested layouts are not recommended"
                            Code = "FA001"
                            Severity = Severity.Warning
                            Range = r
                            Fixes = []
                        }
                    )
                    |> Seq.toList
        }