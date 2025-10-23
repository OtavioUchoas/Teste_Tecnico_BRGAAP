sap.ui.define([
	"sap/ui/core/ComponentContainer"
], (ComponentContainer) => {
	"use strict";

	new ComponentContainer({
		name: "root",
        manifest: true,
		settings : {
			id : "walkthrough"
		},
		async: true
	}).placeAt("content");
});