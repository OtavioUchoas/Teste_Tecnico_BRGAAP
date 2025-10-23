sap.ui.define([

], function () {
    "use strict";

    return {
        obterTarefas({ title = "" } = {}) {
            let searchParams = new URLSearchParams(arguments[0] || {});
            searchParams.forEach((value, key) => {
                if (value !== 0 && value == '')
                    searchParams.delete(key);
            });

            const searchParamsString = searchParams.toString();

            return new Promise(function (resolve, reject) {
                fetch("/todos" + (searchParamsString ? ("?" + searchParamsString) : ""))
                    .then(function(response) {
                        if (!response.ok) {
                            throw new Error(response.statusText);
                        }
                        return response.json();
                    })
                    .then(function(json) {
                        resolve(json);
                    })
                    .catch(function(error) {
                        reject(error.message);
                    })
            });
        },

        obterTarefaPorId(tarefaId){
            return new Promise(function(resolve, reject) {
                fetch(`/todos/${tarefaId}`)
                    .then(function (response) {
                        if (!response.ok) {
                            throw new Error(response.statusText);
                        }
                        return response.json();
                    })
                    .then(function(json) {
                        resolve(json);
                    })
                    .catch(function(error) {
                        reject(error.message);
                    })
            });
        },

        atualizarTarefaStatusCompleted(tarefaId){
            return new Promise(function (resolve, reject) {
                fetch(`/todos/${tarefaId}`, { method: "PUT" })
                    .then(function (response) {
                        if (!response.ok) {
                            if (response.status === 400){
                                return response.text();
                            }
                            throw new Error(response.statusText);
                        }
                        resolve();
                    })
                    .then(function(json) {
                        reject(json);
                    })
                    .catch(function(error) {
                        reject(error.message);
                    })
            });
        }
    }
});