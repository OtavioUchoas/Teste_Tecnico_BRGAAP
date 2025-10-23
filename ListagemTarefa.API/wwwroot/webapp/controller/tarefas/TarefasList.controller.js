sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/json/JSONModel",
    "root/services/tarefas/TarefasServico",
    "sap/m/MessageToast"
], (Controller, JSONModel, TarefasServico, MessageToast) => {
    "use strict";

    const pageSizeData = [
        10, 20, 50
    ];

    const orderColumnData = [
        "id", "title", "userId", "completed"
    ];

    const orderTypes = [
        "asc", "desc"
    ];

    return Controller.extend("root.controller.tarefas.TarefasList", {

        onInit() {
            this.getView().addStyleClass(this.getOwnerComponent().getContentDensityClass());

            this.tarefasModel = new JSONModel([]);
            this.getView().setModel(this.tarefasModel, "tarefas");

            const pageSizeModel = new JSONModel(pageSizeData);
            this.getView().setModel(pageSizeModel, "pageSize");

            const orderColumnModel = new JSONModel(orderColumnData);
            this.getView().setModel(orderColumnModel, "orderColumn");

            const orderTypeModel = new JSONModel(orderTypes);
            this.getView().setModel(orderTypeModel, "orderType");

            this.paginationModel = new JSONModel({
                page: 1,
                pageSize: 10,
                minPage: 1,
                maxPage: Number.MAX_SAFE_INTEGER,
                orderColumn: "id",
                orderType: "asc",
                search: "",
            });
            this.getView().setModel(this.paginationModel, "pagination");

            const oRouter = this.getOwnerComponent().getRouter();
			oRouter.getRoute("overview").attachPatternMatched(this.onPatternMatched, this);            
        },

        onPatternMatched(oEvent) {
            this.paginationModel.setData({
                page: 1,
                pageSize: 10,
                minPage: 1,
                maxPage: Number.MAX_SAFE_INTEGER,
                orderColumn: "id",
                orderType: "asc",
                search: "",
            });
            this.listarTarefas();
		},

        listarTarefas() {
            const tarefasModel = this.tarefasModel;
            const view = this.getView();
            const paginationModel = this.paginationModel;
            const page = paginationModel.getProperty("/page");
            const pageSize = paginationModel.getProperty("/pageSize");
            const search = paginationModel.getProperty("/search");
            const orderColumn = paginationModel.getProperty("/orderColumn");
            const orderType = paginationModel.getProperty("/orderType");
            const urlParams = {
                title: search,
                page: page,
                pageSize: pageSize,
                sort: orderColumn,
                order: orderType,
            };
            const oResourceBundle = this.getView().getModel("i18n").getResourceBundle();
            view.setBusy(true);
            TarefasServico.obterTarefas(urlParams)
                .then(function (response) {
                    if (page > 1 && !response?.length){
                        paginationModel.setProperty("/page", page - 1);
                        paginationModel.setProperty("/maxPage", page - 1);
                        const message = oResourceBundle.getText("noMoreRecords");
                        MessageToast.show(message);
                    } else {
                        if (page === 1 && !response?.length){
                            paginationModel.setProperty("/maxPage", 1);
                            const message = oResourceBundle.getText("noMoreRecords");
                            MessageToast.show(message);
                        }
                        tarefasModel.setData(response);
                    }
                })
                .catch(function (error) {
                    MessageToast.show(error);
                })
                .finally(function(){
                    view.setBusy(false);
                });
        },

        _debounceTimer: null,

        onLiveChangeTarefas: function(oEvent) {
            const sQuery = oEvent.getParameter("newValue");

            if (this._debounceTimer) {
                clearTimeout(this._debounceTimer);
            }

            this._debounceTimer = setTimeout(() => {
                this._executeSearch(sQuery);
            }, 500);
        },

        _executeSearch: function(sQuery) {
            this.paginationModel.setProperty("/page", 1);
            this.paginationModel.setProperty("/maxPage", Number.MAX_SAFE_INTEGER);
            this.paginationModel.setProperty("/search", sQuery);
            this.listarTarefas();
        },

        onSearchTarefas: function(oEvent) {
            const sQuery = oEvent.getParameter("query");
            this._executeSearch(sQuery);
        },

        onPressPreviousPage: function(){
            const page = this.paginationModel.getProperty("/page");
            this.paginationModel.setProperty("/page", page - 1);
            this.listarTarefas();
        },

        onPressNextPage: function(){
            const page = this.paginationModel.getProperty("/page");
            this.paginationModel.setProperty("/page", page + 1);
            this.listarTarefas();
        },

        onPressTarefasDetail(oEvent) {
            const oItem = oEvent.getSource();
            const oData = oItem.getBindingContext("tarefas").getObject();
            const id = oData.id;
            const oRouter = this.getOwnerComponent().getRouter();
            oRouter.navTo("tarefasDetail", {
                tarefaId: window.encodeURIComponent(id)
            });
        },

        onSelectChangePaginationData: function(){
            this.paginationModel.setProperty("/page", 1);
            this.paginationModel.setProperty("/maxPage", Number.MAX_SAFE_INTEGER);
            this.listarTarefas();
        }

    });
});