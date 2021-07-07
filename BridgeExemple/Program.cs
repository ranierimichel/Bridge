using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using static System.Console;

namespace BridgeExemple
{
    public interface IRenderer
    {
        void RenderCircle(float radius);
        void RenderSquare(float side);
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing a circle of radius {radius}");
        }

        public void RenderSquare(float side)
        {
            WriteLine($"Drawing a square of side {side}");
        }
    }

    public class RasterRender : IRenderer
    {
        public void RenderCircle(float radius)
        {
            WriteLine($"Drawing pixels for circle with radius {radius}");
        }

        public void RenderSquare(float side)
        {
            WriteLine($"Drawing pixels for square with side {side}");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;
        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(paramName: nameof(renderer));
        }
        public abstract void Draw();
        public abstract void Resize(float factor);
    }
    public class Circle : Shape
    {
        private float radius;
        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }
        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }
        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    public class Square : Shape
    {
        private float side;

        public Square(IRenderer renderer, float side) : base(renderer)
        {
            this.side = side;
        }

        public override void Draw()
        {
            renderer.RenderSquare(side);
        }

        public override void Resize(float factor)
        {
            side *= factor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var render = new VectorRenderer();
            var square = new Square(render, 3);
            square.Draw();
            square.Resize(4.0f);
            square.Draw();

            var cb = new ContainerBuilder();
            cb.RegisterType<VectorRenderer>().As<IRenderer>().SingleInstance();
            cb.Register((c, p) =>
                new Circle(c.Resolve<IRenderer>(),
                    p.Positional<float>(0)));

            using(var c = cb.Build())
            {
                var circle = c.Resolve<Circle>(
                    new PositionalParameter(0, 5.0f)
                );
                circle.Draw();
                circle.Resize(2.0f);
                circle.Draw();
            }

            var cb1 = new ContainerBuilder();
            cb1.RegisterType<RasterRender>().As<IRenderer>().SingleInstance();
            cb1.Register((c, p) =>
                new Square(c.Resolve<IRenderer>(),
                p.Positional<float>(0)));

            using(var c = cb1.Build())
            {
                var square1 = c.Resolve<Square>(
                    new PositionalParameter(0, 3.0f)
                    );
                square1.Draw();
                square1.Resize(4.0f);
                square1.Draw();
            }
        }
    }
}
