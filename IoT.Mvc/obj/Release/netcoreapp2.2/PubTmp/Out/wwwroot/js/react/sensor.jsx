class Hello extends React.Component {
    render() {

        return (
            <div className="card">
                <div className="card-body pb-0">
                    <div className="h1 fw-bold float-right text-danger">+</div>
                    <h2 className="mb-2">27</h2>
                    <p className="text-muted">Humidity sensor</p>
                    <div className="pull-in sparkline-fix">
                        <div id="lineChart2"></div>
                    </div>
                </div>
            </div>

        );
    }
}
ReactDOM.render(
    <Hello />,
    document.getElementById("content")
);