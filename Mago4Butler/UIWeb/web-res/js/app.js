//=============================================================================
function UIController(backendService, model) {
    this.viewModel = new ViewModel();
    this.backendService = backendService;
    this.backendModel = model;
}

UIController.prototype.install = function () {
    this.backendService.command.type = 'Install';
    this.backendService.command.instanceIndex = -1;
    this.backendService.execute();
}
UIController.prototype.uninstall = function () {
    this.backendService.command.type = 'Remove';
    this.backendService.command.instanceIndex = 0;
    this.backendService.execute();
}
UIController.prototype.update = function () {
    this.backendService.command.type = 'Update';
    this.backendService.command.instanceIndex = 0;
    this.backendService.execute();
}

UIController.prototype.init = function () {
    this.viewModel.init(this.backendModel);

    var instancesDivContent = '<ul>';
    if (this.viewModel.instances.length > 0) {
        for (var i = 0; i < this.viewModel.instances.length; i++) {
            instancesDivContent += '<li>' + this.viewModel.instances[i].name + '</li>';
        }
    }
    instancesDivContent += '</ul>';

    document.getElementById("instancesDiv").innerHTML = instancesDivContent;
}

//=============================================================================
function ViewModel() {
    this.instances = [];
}
ViewModel.prototype.init = function (model) {
    if (model.instances.length > 0) {
        for (var i = 0; i < model.instances.length; i++) {
            var inst = model.getInstance(i);
            this.instances.push(new Instance(inst.name, inst.version, inst.installedOn, inst.edition))
        }
    }
}
ViewModel.prototype.addInstance = function (instance) {
    this.instances.push(instance);
}
ViewModel.prototype.updateInstance = function (instance) {
    this.instances.push(instance);
}
ViewModel.prototype.removeInstance = function (instance) {
    this.instances.pop(instance);
}

//=============================================================================
function Instance(name, version, installedOn, edition) {
    this.name = name;
    this.version = version;
    this.installedOn = installedOn;
    this.edition = edition;
}