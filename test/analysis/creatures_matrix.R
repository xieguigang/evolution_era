require(evolution_era);
require(GCModeller);

imports "Analysis" from "Evolution";
imports "geneExpression" from "phenotype_kit";

let result = Analysis::open(file = `${@dir}/../demo.dat`);
let creatures = Analysis::creatures(result);

print(creatures, max.print = 6);

creatures
|> load.expr()
|> write.expr_matrix(file = `${@dir}/creatures.HTS`, 
binary = TRUE);