require(GCModeller);

imports "geneExpression" from "phenotype_kit";
imports "umap" from "MLkit";

let rawdata = `${@dir}/creatures.HTS`
|> load.expr0()
|> as.data.frame()
;
let era = rawdata$Era;

rawdata[, "None"] = NULL;
rawdata[, "Era"] = NULL;

rownames(rawdata) = `${era} - ${rownames(rawdata)}`;

# print(rawdata);

# "labels", "umap"
let embedded = umap(rawdata, dimension = TRUE);

embedded = as.data.frame(embedded$umap, labels = embedded$labels);

print(embedded);

write.csv(embedded, file = `${@dir}/creatures_umap.csv`);