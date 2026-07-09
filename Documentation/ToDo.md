## Introduction

This file is meant as a way to keep track of all tasks that could be executed as well as to make clear what tasks are currently the focus.

## Current Task

Currently the priority is to work on adapting the application so that it can run inside of a docker container.

### Components

* Replace the json file based palette repository with sql dbs hosted in a docker container.

* Create an ImageRepository to hold data on generated images.
  * Create a docker based db for images.
  * Remove image data holding element of ImageService.
