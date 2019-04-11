using System;
using System.Collections.Generic;
using System.Linq;

public class Reactor
{
    public InputCell CreateInputCell(int value) => new InputCell(value);

    public ComputeCell CreateComputeCell(IEnumerable<Cell> producers, Func<int[], int> compute) =>
        new ComputeCell(producers, compute);
}

public abstract class Cell
{
    public event System.EventHandler<int> Update;
    public event System.EventHandler<int> Changed;
    protected void Notify()
    {
        if (Update != null) 
            Update(this, this.Value);
        if (Changed != null)
            Changed(this, this.Value);
    }

    public virtual int Value { get; set; }
}

public class InputCell : Cell
{
    public InputCell(int value) => this.Value = value;
    public override int Value
    {
        get => base.Value;
        set
        {
            base.Value = value;
            Notify();
        }
    }
}

public class ComputeCell : Cell
{
    private int LastKnownValue;
    private Func<int[], int> Compute { get; }
    private Cell[] Producers { get; }
    private int[] Inputs => Producers.Select(p => p.Value).ToArray();

    public ComputeCell(IEnumerable<Cell> producers, Func<int[], int> compute)
    {
        this.Producers = producers.ToArray();
        this.Compute = compute;
        Calculate();
        this.LastKnownValue = this.Value;
        foreach (var producer in producers)
            SubscribeTo(producer);
    }

    private void SubscribeTo(Cell producer)
    {
        producer.Update += this.OnUpdate;
        producer.Changed += this.OnChange;
    }

    private void Calculate() => this.Value = Compute(Inputs);

    private void OnUpdate(object sender, int newValue) => Calculate();

    private void OnChange(object sender, int newValue)
    {
        if (this.LastKnownValue != this.Value)
        {
            this.LastKnownValue = this.Value;
            Notify();
        }
    }
}
