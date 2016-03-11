﻿Imports System.Text
Imports RDotNET.Extensions.VisualBasic
Imports RDotNET.Extensions.VisualBasic.base
Imports RDotNET.Extensions.VisualBasic.Services.ScriptBuilder
Imports RDotNET.Extensions.VisualBasic.utils.read.table

Namespace WGCNA.App

    Public Class WGCNA_samples : Inherits WGCNA

        Public Property readData As readTableAPI

        Public Property LocusMap As String

        Const myData As String = "myData"
        Const datExpr As String = "datExpr"

        Protected Overrides Function __R_script() As String
            Dim sbr As ScriptBuilder =
                New ScriptBuilder(4096) + New options With {
                    .stringsAsFactors = False
            }

            sbr += myData <= readData
            sbr += [dim](myData)
            sbr += names(myData)
            sbr += datExpr <= [as].data.frame(t(myData[, -c(1)]))
            names(datExpr) = myData$inputID  
rownames(datExpr) = names(myData)[-c(1)]  
gsg = goodSamplesGenes(datExpr, verbose = 3)
            If (!gsg$allOK)  
{  
If (Sum(!gsg$goodGenes) > 0) Then
                    printFlush(paste("Removing genes:", paste(names(datExpr)[!gsg$goodGenes], collapse = ", ")))
                    If (Sum(!gsg$goodSamples) > 0) Then
                        printFlush(paste("Removing samples:", paste(rownames(datExpr)[!gsg$goodSamples], collapse = ", ")))
                        datExpr = datExpr[gsg$goodSamples, gsg$goodGenes]  
}  
Write.table(names(datExpr)[!gsg$goodGenes], file = "Out/removeGene.xls", row.names = False, col.names = False, quote = False)
                        Write.table(names(datExpr)[!gsg$goodSamples], file = "Out/removeSample.xls", row.names = False, col.names = False, quote = False)
                        sampleTree = flashClust(dist(datExpr), method = "average") #根据样本表达量使用平均距离法建树  
pdf(file = "Out/sampleClustering.pdf", width = 12, height = 9)
                        par(cex = 0.6)
                        par(mar = c(0, 4, 2, 0))
                        plot(sampleTree, Main() = "Sample clustering", Sub() = "", xlab = "", cex.lab = 1.5, cex.axis = 1.5, cex.main = 2)
                        dev.off()
                        save(datExpr, file = "dataInput.RData")
        End Function
    End Class
End Namespace