using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeExercise
{
    public interface IRenderer
    {
        string WhatToRenderAs { get; }
    }
    public class VectorRenderer : IRenderer
    {
        public string WhatToRenderAs => "as lines";
    }
    public class RasterRender : IRenderer
    {
        public string WhatToRenderAs => "as pixels";
    }

    public abstract class Shape
    {
        protected IRenderer renderer;
        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(paramName: nameof(renderer));
        }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Drawing {Name} {renderer.WhatToRenderAs}";
        }
    }

    public class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer)
        {
            Name = "Triangle";
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer)
        {
            Name = "Square";
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Triangle(new RasterRender()).ToString());
            Console.WriteLine(new Square(new VectorRenderer()).ToString());
        }
    }
}
