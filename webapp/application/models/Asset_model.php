<?php
class Asset_model extends CI_Model {

    public $image;
    public $prefab;
    public $name;
    public $build_time;
    public $build_points;
    public $generate_time;
    public $generate_points;

    public function __construct()
    {
        parent::__construct();
    }

    public function get_all()
    {
		$query = $this->db->get('assets');
		return $query->result();
    }

    public function get_one($id)
    {
		$this->db->where('id', $id);
		$query = $this->db->get('assets');
		return $query->result();
    }

    public function update()
    {
        $id                     = $this->input->post('id');
        $this->image            = $this->input->post('image');
        $this->prefab           = $this->input->post('prefab');
        $this->name             = $this->input->post('name');
        $this->build_time       = $this->input->post('build_time');
        $this->build_points     = $this->input->post('build_points');
        $this->generate_time    = $this->input->post('generate_time');
        $this->generate_points  = $this->input->post('generate_points');

        $this->db->update('assets', $this, array('id' => $id));
    }
}
?>
