sap.ui.define([
	"sap/ui/core/mvc/Controller",
    "sap/ui/core/routing/History",
    "sap/m/MessageToast",
    "sap/ui/model/json/JSONModel",
    "root/services/tarefas/TarefasServico"
], (Controller, History, MessageToast, JSONModel, TarefasServico) => {
	"use strict";

	return Controller.extend("root.controller.tarefas.TarefasDetail", {
		onInit() {
            this.tarefaModel = new JSONModel();
			this.getView().setModel(this.tarefaModel, "tarefa");
            
			const oRouter = this.getOwnerComponent().getRouter();
			oRouter.getRoute("tarefasDetail").attachPatternMatched(this.onPatternMatched, this);
		},

		onPatternMatched(oEvent) {
            this.tarefaModel.setData({});
            const tarefaId = window.decodeURIComponent(oEvent.getParameter("arguments").tarefaId)
            this.carregarTarefa(tarefaId);
		},

        carregarTarefa(tarefaId){
            const tarefaModel = this.tarefaModel;
            const view = this.getView();
            view.setBusy(true);
            TarefasServico.obterTarefaPorId(tarefaId)
                .then(function (response) {
                    tarefaModel.setData(response);
                })
                .catch(function (error) {
                    debugger;
                })
                .finally(function(){
                    view.setBusy(false);
                });
        },

		onNavBack() {
			const oHistory = History.getInstance();
			const sPreviousHash = oHistory.getPreviousHash();

			if (sPreviousHash !== undefined) {
				window.history.go(-1);
			} else {
				const oRouter = this.getOwnerComponent().getRouter();
				oRouter.navTo("overview", {}, true);
			}
		},

        onSwitchChangeTarefaCompleted: function(oEvent){
            const tarefaModel = this.tarefaModel;
            const tarefaId = tarefaModel.getProperty("/id");
            const state = oEvent.getParameter("state");
            const view = this.getView();
            const oResourceBundle = this.getView().getModel("i18n").getResourceBundle();
            view.setBusy(true);
            TarefasServico.atualizarTarefaStatusCompleted(tarefaId)
                .then(function(response) {
                    const message = oResourceBundle.getText("successfulOperation");
                    MessageToast.show(message);
                })
                .catch(function(error) {
                    tarefaModel.setProperty("/completed", !state);
                    MessageToast.show(error);
                })
                .finally(function(){
                    view.setBusy(false);
                });
        }
	});
});