<?php
class Mwo_model extends CI_Model {

    public $user;
    public $world;
    public $prefab;
    public $name;
    public $x;
    public $y;
    public $z;
    public $rotation;
    public $node;
    public $portal;
    public $progress;

    public function __construct() {
        parent::__construct();
    }

    public function get_one($id) {
		$this->db->where('world', $id);
		$query = $this->db->get('mwo');
		return $query->result();
    }

    public function get_one_object($id) {
        $this->db->where('id', $id);
		$query = $this->db->get('mwo');
		return $query->result();
    }

    public function get_prefab_count($prefab) {
        $this->db->where('prefab', $prefab);
        $query = $this->db->get('mwo');
        return $query->num_rows();
    }

    public function get_all_prefab_count() {
        $query = $this->db->get('mwo');
        return $query->num_rows();
    }

    public function add() {
        $this->user = $this->input->post('user');
        $this->world = $this->input->post('world');
        $this->prefab = $this->input->post('prefab');
        $this->name = $this->input->post('name');
        $this->x = $this->input->post('x');
        $this->y = $this->input->post('y');
        $this->z = $this->input->post('z');
        $this->rotation = $this->input->post('rotation');
        $this->node = $this->input->post('node');
        $this->portal = $this->input->post('portal');
        $this->progress = $this->input->post('progress');
        $this->db->insert('mwo', $this);

        return $this->db->insert_id();
    }

    public function update() {
        $id = $this->input->post('id');

        $this->user = $this->input->post('user');
        $this->world = $this->input->post('world');
        $this->prefab = $this->input->post('prefab');
        $this->name = $this->input->post('name');
        $this->x = $this->input->post('x');
        $this->y = $this->input->post('y');
        $this->z = $this->input->post('z');
        $this->rotation = $this->input->post('rotation');
        $this->node = $this->input->post('node');
        $this->portal = $this->input->post('portal');
        $this->progress = $this->input->post('progress');

        $this->db->update('mwo', $this, array('id' => $id));

        return true;
    }

    public function delete($id) {
        $id = $this->input->post('id');
        $this->db->delete('mwo', array('id' => $id));

        return true;
    }
}
?>
