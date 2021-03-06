﻿using g3;
using gs;
using Sutro.Core.Decompilers;
using Sutro.Core.Models.GCode;
using Sutro.PathWorks.Plugins.API.Visualizers;
using Sutro.PathWorks.Plugins.Core.CustomData;
using Sutro.PathWorks.Plugins.Core.Meshers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sutro.PathWorks.Plugins.Core.Visualizers
{
    public abstract class VisualizerBase<TPrintVertex> : IVisualizer where TPrintVertex : IToolpathVertex
    {
        protected readonly DecompilerBase<TPrintVertex> decompiler;
        protected readonly IToolpathPreviewMesher<TPrintVertex> mesher;
        protected readonly FillTypeMapper fillTypeMapper;

        // Track current properties
        protected int layerIndex;

        protected int pointCount;
        protected int fillTypeInteger;
        protected Vector3f color;

        public string Name { get; }

        public VisualizerBase(
            string name,
            FillTypeMapper fillTypeMapper,
            DecompilerBase<TPrintVertex> decompiler,
            IToolpathPreviewMesher<TPrintVertex> mesher)
        {
            Name = name;
            this.fillTypeMapper = fillTypeMapper;
            this.decompiler = decompiler;
            this.mesher = mesher;
        }

        public Dictionary<int, VisualizerFillType> FillTypes => fillTypeMapper.VisualizerFillTypes;

        public abstract VisualizerCustomDataDetailsCollection CustomDataDetails { get; }

        public event Action<ToolpathPreviewVertex[], int[], int> OnMeshGenerated;

        public event Action<List<Vector3d>, int> OnLineGenerated;

        public event Action<ToolpathPreviewVertex[], int> OnPointsGenerated;

        public event Action<double, int> OnNewPlane;

        protected virtual void EndEmit(Tuple<ToolpathPreviewVertex[], int[]> mesh, int layerIndex)
        {
            OnMeshGenerated?.Invoke(mesh.Item1, mesh.Item2, layerIndex);
        }

        protected void RaiseLineGenerated(List<Vector3d> points, int layerIndex)
        {
            OnLineGenerated?.Invoke(points, layerIndex);
        }

        protected void RaisePointsGenerated(ToolpathPreviewVertex[] points, int layerIndex)
        {
            OnPointsGenerated?.Invoke(points, layerIndex);
        }

        protected void RaiseNewLayer(int newLayerIndex, double newLayerHeight = 0)
        {
            layerIndex = newLayerIndex;
            OnNewPlane?.Invoke(newLayerHeight, newLayerIndex);
        }

        protected IEnumerable<IVisualizerCustomDataDetails> EnumerateCustomFields()
        {
            if (CustomDataDetails.Field0 != null)
                yield return CustomDataDetails.Field0;

            if (CustomDataDetails.Field1 != null)
                yield return CustomDataDetails.Field1;

            if (CustomDataDetails.Field2 != null)
                yield return CustomDataDetails.Field2;

            if (CustomDataDetails.Field3 != null)
                yield return CustomDataDetails.Field3;

            if (CustomDataDetails.Field4 != null)
                yield return CustomDataDetails.Field4;

            if (CustomDataDetails.Field5 != null)
                yield return CustomDataDetails.Field5;
        }

        protected virtual void EmitMesh(LinearToolpath3<TPrintVertex> toolpath)
        {
            if (toolpath.VertexCount < 2)
                return;

            var mesh = mesher.Generate(toolpath, VertexFactory);
            pointCount += toolpath.VertexCount;

            EndEmit(Tuple.Create(mesh.Vertices.ToArray(), mesh.Triangles.ToArray()), layerIndex);
        }

        protected abstract ToolpathPreviewVertex VertexFactory(TPrintVertex printVertex, Vector3d position, float brightness);

        protected void Reset()
        {
            layerIndex = 0;
            pointCount = 0;

            foreach (var customData in EnumerateCustomFields())
            {
                if (customData is AdaptiveRange adaptive)
                {
                    adaptive.Reset();
                }
            }
        }

        public virtual void BeginGCodeLineStream()
        {
            Reset();
            decompiler.Begin();
        }

        public virtual void ProcessGCodeLine(GCodeLine line)
        {
            decompiler.ProcessGCodeLine(line);
        }

        public virtual void EndGCodeLineStream()
        {
            decompiler.End();
        }

        protected virtual void RaiseLineGenerated(LinearToolpath3<TPrintVertex> toolpath)
        {
            var points = new List<Vector3d>(toolpath.VertexCount);
            foreach (var vertex in toolpath)
            {
                points.Add(vertex.Position);
            }

            RaiseLineGenerated(points, layerIndex);
        }
    }
}