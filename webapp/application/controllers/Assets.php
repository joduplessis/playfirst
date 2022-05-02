<?php
defined('BASEPATH') OR exit('No direct script access allowed');

class Assets extends CI_Controller {

	function __construct() {
		parent::__construct();
	}

	public function index() {
		$this->load->model('Asset_model');
		$this->load->model('Mwo_model');

		$output = $this->Asset_model->get_all();

		$data["assets"] = $output;
		$data["heading"] = "Assets";

		$amountOfObjectsPlaced = $this->Mwo_model->get_all_prefab_count();

		// Get the count for the placed
		foreach ($data["assets"] as $asset) {
			$asset->{"placed"} =  $this->Mwo_model->get_prefab_count($asset->prefab);
			$asset->{"placed_percent"} = round(($this->Mwo_model->get_prefab_count($asset->prefab) / $amountOfObjectsPlaced * 100), 2);
		}

		$this->load->view('header');
		$this->load->view('navigation');
		$this->load->view('assets', $data);
		$this->load->view('footer');
	}

	public function asset() {
		$id = $this->uri->segment(3);

		$this->load->model('Asset_model');

		$output = $this->Asset_model->get_one($id);

		$data["asset"] = $output;

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('asset');
		$this->load->view('footer');
	}

	public function update() {
    $this->load->model('Asset_model');
    $this->Asset_model->update();

		redirect('assets?success=true', 'location');
  }


}
