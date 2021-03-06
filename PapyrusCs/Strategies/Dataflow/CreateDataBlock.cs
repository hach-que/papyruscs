﻿using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Maploader.World;
using PapyrusCs.Database;

namespace PapyrusCs.Strategies.Dataflow
{
 
    public class ChunkAndData
    {
        public Chunk C { get; }
        public List<SubChunkData> Cd { get; }
        

        public ChunkAndData(Chunk c, ChunkData cd)
        {
            C = c;
            Cd = cd.SubChunks;
            foreach (var subChunkData in Cd)
            {
                subChunkData.ClearData();
            }
        }
    }
    public class CreateDataBlock : ITplBlock
    {
        public TransformBlock<IEnumerable<ChunkData>, IEnumerable<ChunkAndData>> Block { get; }

        public CreateDataBlock(World world, ExecutionDataflowBlockOptions options)
        {
            Block = new TransformBlock<IEnumerable<ChunkData>, IEnumerable<ChunkAndData>>(chunkDatas =>
            {
                var chunks = new List<ChunkAndData>();
                foreach (var cd in chunkDatas)
                {
                    chunks.Add(new ChunkAndData(world.GetChunk(cd.X, cd.Z, cd), cd));
                }

                ProcessedCount++;
                return chunks;
            }, options);
        }

        public int InputCount => Block.InputCount;
        public int OutputCount => Block.OutputCount;
        public int ProcessedCount { get; set; }
    }
}