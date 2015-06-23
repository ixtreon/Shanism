namespace ShanoEditorSyntax

type Option<'T> = 
    val public HasValue : bool
    val public Value : 'T

    new() = { HasValue = false; Value = Unchecked.defaultof<'T> }

    new(value) = { HasValue = false; Value = value }