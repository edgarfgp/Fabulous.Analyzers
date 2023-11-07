module Fabulous.Analyzers.NestedLayoutsAnalyzer

open FSharp.Analyzers.SDK
open FSharp.Analyzers.SDK.ASTCollecting
open FSharp.Compiler.Syntax
open FSharp.Compiler.Text

[<CliAnalyzer("NestedLayoutsAnalyzer",
          "Analyzes the code for nested layouts",
          "")>]
let nestedLayoutsAnalyzer: Analyzer<CliContext> =
    fun (ctx: CliContext) ->
        async {
            let state = ResizeArray<range>()

            let walker =
                { new SyntaxCollectorBase() with
                    override this.WalkBinding(biding: SynBinding) =
                        ()

                }
            
            walkAst walker ctx.ParseFileResults.ParseTree

            return
                state
                |> Seq.map (fun r ->
                    {
                        Type = "NestedLayoutsAnalyzer"
                        Message = "Nested layouts are not recommended"
                        Code = "FA001"
                        Severity = Warning
                        Range = r
                        Fixes = []
                    }
                )
                |> Seq.toList
    }