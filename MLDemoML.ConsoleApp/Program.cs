//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using MLDemoML.Model.DataModels;


namespace MLDemoML.ConsoleApp
{
    class Program
    {
        //机器学习模型，用于加载和预测
        private const string MODEL_FILEPATH = @"MLModel.zip";

        //用于预测的数据集
        private const string DATA_FILEPATH = @"C:\Users\wangjifeng\Desktop\data.tsv";

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            //由ML.NET CLI和AutoML用于生成模型的训练代码
            //ModelBuilder.CreateModel();

            ITransformer mlModel = mlContext.Model.Load(GetAbsolutePath(MODEL_FILEPATH), out DataViewSchema inputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            // 创建示例数据来对其进行单个预测
            ModelInput sampleData = CreateSingleDataSample(mlContext, DATA_FILEPATH);

            // 尝试一个预测
            ModelOutput predictionResult = predEngine.Predict(sampleData);

            Console.WriteLine($"Single Prediction --> Actual value: {sampleData.Sentiment} | Predicted value: {predictionResult.Prediction}");

            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }

        // 方法加载单行数据以尝试单个预测
        // 你可以修改这段代码，并在这里创建你自己的样本数据(硬编码的或来自任何来源的)
        private static ModelInput CreateSingleDataSample(MLContext mlContext, string dataFilePath)
        {
            // 读取dataset以获得用于尝试预测的单行      
            IDataView dataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            // 在这里(ModelInput对象)，您可以提供新的测试数据，无论是硬编码的还是来自最终用户应用程序的，而不是来自文件的行。
            ModelInput sampleForPrediction = mlContext.Data.CreateEnumerable<ModelInput>(dataView, false)
                                                                        .First();
            return sampleForPrediction;
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
